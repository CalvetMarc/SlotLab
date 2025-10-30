import os
import json
import time
import subprocess
import importlib
from tqdm import tqdm
from dataLoader import load_game_tables
from decimal import Decimal, InvalidOperation
import pandas as pd

# --- .NET Runtime configuration ---
os.environ["PYTHONNET_RUNTIME"] = "coreclr"
os.environ["DOTNET_ROOT"] = "/usr/lib/dotnet"
os.environ["PYTHONNET_CLRLIBRARY"] = "/usr/lib/dotnet/shared/Microsoft.NETCore.App/8.0.21/libcoreclr.so"

import clr
clr.AddReference("/usr/lib/dotnet/shared/Microsoft.NETCore.App/8.0.21/System.Text.Json.dll")
from System.Text.Json.Nodes import JsonNode

# --- Load simulation config file ---
CONFIG_FILE = "simulationConfig.json"

with open(CONFIG_FILE, "r", encoding="utf-8") as f:
    config_json = json.load(f)

simulation = config_json["simulation"]
game_name = simulation["game_name"]
rebuild_engine = simulation["rebuild_engine"]
config_mode = simulation["config_mode"]
overwrite_after_test = simulation["overwrite_stable_after_test"]
N_SPINS = simulation["total_spins"]
BET = simulation.get("bet", 1.0)

print(f"🎮 Loaded simulation config for '{game_name}'")
print(f"   Rebuild engine: {rebuild_engine}")
print(f"   Config mode: {config_mode}")
print(f"   Overwrite stable after test: {overwrite_after_test}")
print(f"   Spins to simulate: {N_SPINS:,}")
print(f"   Bet: {BET}\n")

# --- Optional: rebuild the engine ---
if rebuild_engine:
    print("🔧 Rebuilding SlotLab.Engine project...")
    subprocess.run(["dotnet", "build", "../backend/SlotLab.Engine", "-c", "Debug"], check=True)
    print("✅ Engine build complete!\n")

# --- Choose which config file to use ---
if config_mode == "stable":
    CONFIG_PATH = os.path.abspath(f"../backend/SlotLab.Engine/Games/{game_name}/slotConfig.json")

    if not os.path.exists(CONFIG_PATH):
        raise FileNotFoundError(f"❌ slotConfig.json not found for {game_name} at {CONFIG_PATH}")

    print(f"📄 Loading stable configuration from JSON: {CONFIG_PATH}")
    with open(CONFIG_PATH, "r", encoding="utf-8") as f:
        json_str = f.read()

    json_node = JsonNode.Parse(json_str)
    print(f"✅ JsonNode created from JSON file.\n")

elif config_mode == "test":
    EXCEL_PATH = os.path.abspath(f"Games/{game_name}/slotMaths.xlsx")

    if not os.path.exists(EXCEL_PATH):
        raise FileNotFoundError(f"❌ slotMaths.xlsx not found for {game_name} at {EXCEL_PATH}")

    CONFIG_MATH_PATH = os.path.abspath(f"Games/{game_name}/mathsTables.json")
    if not os.path.exists(CONFIG_MATH_PATH):
        raise FileNotFoundError(f"❌ mathsTables.json not found for {game_name} at {CONFIG_MATH_PATH}")

    print(f"📊 Loading test configuration from Excel: {EXCEL_PATH}")
    excel_dict = load_game_tables(f"Games/{game_name}")
    if not excel_dict:
        raise ValueError("❌ No valid tables found in Excel!")

    json_str = json.dumps(excel_dict, indent=2)
    json_node = JsonNode.Parse(json_str)
    CONFIG_PATH = EXCEL_PATH

else:
    raise ValueError("❌ Invalid 'config_mode'. Must be 'stable' or 'test'.")

print(f"📄 Using configuration source: {CONFIG_PATH}\n")

# --- Load engine DLL ---
DLL_PATH = os.path.abspath("../backend/SlotLab.Engine/bin/Debug/net8.0/SlotLab.Engine.dll")
clr.AddReference(DLL_PATH)

# --- Import .NET types ---
from System import Action
from SlotLab.Engine.Models import GameEnvironmentMode
from SlotLab.Engine.Core.Events import RoundCompleted

# --- Dynamically import the game class ---
engine_namespace = "SlotLab.Engine.Games"
game_module = importlib.import_module(engine_namespace)
GameClass = getattr(game_module, game_name)

# --- Instantiate the game ---
print(f"🎰 Initializing game: {game_name} (Simulation mode)...")
game = GameClass(json_node, GameEnvironmentMode.Simulation)
print("✅ Game instance created.\n")

# --- Simulation tracking variables ---
total_win = Decimal("0")
total_bet = Decimal(str(N_SPINS * BET))
hit_count = 0
spins_done = 0
start_time = time.time()

# --- Helper: safe conversion from System.Decimal to Python Decimal ---
def to_decimal(dotnet_decimal):
    """Safely convert .NET System.Decimal to Python Decimal, tolerant to bad formatting."""
    if dotnet_decimal is None:
        return Decimal("0")

    try:
        s = str(dotnet_decimal).replace(",", ".").strip()
        return Decimal(s)
    except (InvalidOperation, ValueError):
        print(f"⚠️ Warning: could not convert {dotnet_decimal!r} to Decimal, using 0 instead")
        return Decimal("0")


# --- Event handler for payouts ---
def on_round_completed(evt):
    global total_win, hit_count, spins_done
    payout = to_decimal(evt.Metadata.PayoutAmount)
    total_win += payout
    spins_done += 1
    if payout > 0:
        hit_count += 1


# --- Subscribe handler ---
handler = Action[RoundCompleted](on_round_completed)
game.gameEventBus.Subscribe[RoundCompleted](handler)

# --- Start simulation ---
print(f"🎯 Running simulation of {N_SPINS:,} spins...")
game.InitializeFSM()  # Initialize FSM — starts at IdleState

# --- Main simulation loop using Tick() ---
print("⚙️ Starting simulation loop (Tick-based mode)...")
start_time = time.time()

with tqdm(total=N_SPINS, desc="Spinning...", ncols=100) as pbar:
    for i in range(N_SPINS):
        game.Tick()  # 🔁 advances FSM (Spin → Evaluate → Payout → Idle...)
        pbar.update(1)

# --- End of simulation ---
elapsed = time.time() - start_time

# --- Compute and display results ---
rtp = (total_win / total_bet) if total_bet > 0 else 0
hit_rate = hit_count / spins_done if spins_done > 0 else 0
speed = spins_done / elapsed if elapsed > 0 else 0

print("\n✅ Simulation completed!")
print(f"🎰 Spins simulated: {spins_done:,}")
print(f"💰 RTP: {float(rtp)*100:.2f}%")
print(f"🎯 Hit Rate: {hit_rate*100:.2f}%")
print(f"⚡ Speed: {speed:,.0f} spins/s")
print(f"⏱️ Total time: {elapsed:.2f} seconds\n")
