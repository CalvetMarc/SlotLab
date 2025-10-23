import os
import json
import time
import subprocess
import importlib
from tqdm import tqdm
from dataLoader import load_game_tables 
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
BET = simulation["bet"]

print(f"🎮 Loaded simulation config for '{game_name}'")
print(f"   Rebuild engine: {rebuild_engine}")
print(f"   Config mode: {config_mode}")
print(f"   Overwrite stable after test: {overwrite_after_test}\n")

# --- Optional: rebuild the engine ---
if rebuild_engine:
    print("🔧 Rebuilding SlotLab.Engine project...")
    subprocess.run(["dotnet", "build", "../backend/SlotLab.Engine"], check=True)
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

    print(f"📊 Loading test configuration from Excel: {EXCEL_PATH}")

    CONFIG_MATH_PATH = os.path.abspath(f"Games/{game_name}/mathsTables.json")

    if not os.path.exists(CONFIG_MATH_PATH):
        raise FileNotFoundError(f"❌ mathsTables.json not found for {game_name} at {CONFIG_MATH_PATH}")

    print(f"📄 Loading math tables definition from JSON: {CONFIG_MATH_PATH}")

    # 🔹 Automatically load and process all tables based on the JSON instructions
    excel_dict = load_game_tables(f"Games/{game_name}")

    if not excel_dict:
        raise ValueError("❌ No valid tables found in Excel!")

    # 🔹 Convert everything to JSON and create JsonNode for engine input
    json_str = json.dumps(excel_dict, indent=2)
    json_node = JsonNode.Parse(json_str)

    CONFIG_PATH = EXCEL_PATH

else:
    raise ValueError("❌ Invalid 'config_mode'. Must be 'stable' or 'test'.")

print(f"📄 Using configuration source: {CONFIG_PATH}\n")

# --- Load the compiled .NET engine DLL ---
DLL_PATH = os.path.abspath("../backend/SlotLab.Engine/bin/Debug/net8.0/SlotLab.Engine.dll")
clr.AddReference(DLL_PATH)

# --- Dynamically import the game class from the engine ---
engine_namespace = f"SlotLab.Engine.Games"
game_module = importlib.import_module(engine_namespace)
GameClass = getattr(game_module, game_name)

# --- Create an instance of the game ---
print(f"🎰 Initializing game: {game_name}...")
game = GameClass(json_node)
print(f"✅ Starting simulation with {N_SPINS:,} spins at bet {BET}\n")

# --- Simulation tracking variables ---
total_win = 0.0
total_bet = N_SPINS * BET
hit_count = 0

start_time = time.time()

# --- Main simulation loop ---
for _ in tqdm(range(N_SPINS), desc="Spinning..."):
    play = game.gameBase.Play(BET)
    win = play.TotalWin
    total_win += win
    if win > 0:
        hit_count += 1

elapsed = time.time() - start_time

# --- Results calculation ---
rtp = total_win / total_bet
hit_rate = hit_count / N_SPINS
speed = N_SPINS / elapsed

print("\n✅ Simulation completed!")
print(f"🎰 Spins simulated: {N_SPINS:,}")
print(f"💰 RTP: {rtp*100:.2f}%")
print(f"🎯 Hit Rate: {hit_rate*100:.2f}%")
print(f"⚡ Speed: {speed:,.0f} spins/s")
print(f"⏱️  Total time: {elapsed:.2f} seconds")

# --- Optional overwrite stable config ---
if overwrite_after_test and config_mode == "test":
    stable_path = os.path.abspath(f"../backend/SlotLab.Engine/Games/{game_name}/slotConfig.json")
    print(f"\n💾 Overwriting stable config at: {stable_path}")
    import shutil

    # Save the generated JSON configuration (same content used by the engine)
    with open(stable_path, "w", encoding="utf-8") as f:
        f.write(json_str)

    print("✅ Stable config updated successfully.")
