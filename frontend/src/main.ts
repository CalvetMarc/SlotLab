import { Application, Graphics, Text } from "pixi.js";
import * as signalR from "@microsoft/signalr";
import { DebugPanel } from "./ui/DebugPanel";
import { SpinResult } from "./types/SpinResult";

// --- Crear app Pixi ---
const app = new Application({
  width: window.innerWidth,
  height: window.innerHeight,  // 🔹 Fa que ocupi tota la finestra i es redimensioni automàticament
  background: "#000000",
  antialias: true,
});
await app.init();

document.body.appendChild(app.canvas);

// --- Panell de debug ---
const panel = new DebugPanel();
app.stage.addChild(panel.container);

// --- Crear botó SPIN ---
const spinButton = new Graphics()
  .roundRect(0, 0, 140, 60, 10)
  .fill(0x0088ff); // color blau

const label = new Text({
  text: "SPIN",
  style: { fill: 0xffffff, fontSize: 28, fontWeight: "bold" },
});
label.anchor.set(0.5);
label.position.set(70, 30);
spinButton.addChild(label);

// Posiciona el botó
spinButton.position.set(320, 400);
spinButton.eventMode = "static"; // activem events interactius
spinButton.cursor = "pointer";

// Afegim a l'escena
app.stage.addChild(spinButton);

// --- Connexió SignalR ---
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5207/gamehub")
  .configureLogging(signalR.LogLevel.Information)
  .build();

connection.on("SpinResult", (result: SpinResult) => {
  panel.setMessage(`🎯 Resultat rebut: ${JSON.stringify(result)}`);
});

// --- Funció connectar ---
async function connect() {
  try {
    await connection.start();
    panel.setStatus("✅ Connectat al GameHub");
  } catch (err) {
    panel.setStatus("❌ Error de connexió: " + err);
  }
}

connect();

// --- Interacció del botó SPIN ---
spinButton.on("pointerover", () => {
  spinButton.tint = 0x00aaff; // canvia lleugerament de color
});
spinButton.on("pointerout", () => {
  spinButton.tint = 0xffffff; // restaura
});

spinButton.on("pointerdown", async () => {
  panel.setMessage("🎰 Enviant spin...");
  try {
    await connection.invoke("Spin", 1.0);
  } catch (err) {
    panel.setMessage("❌ Error enviant spin: " + err);
  }
});
window.addEventListener("resize", () => {
  app.renderer.resize(window.innerWidth, window.innerHeight);
});

