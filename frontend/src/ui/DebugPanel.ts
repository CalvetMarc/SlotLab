import { Container, Text } from "pixi.js";

export class DebugPanel {
  container: Container;
  private statusText: Text;
  private messageText: Text;

  constructor() {
    this.container = new Container();

    // Text d’estat (connexió)
    this.statusText = new Text({
      text: "Connectant...",
      style: {
        fill: 0x00ffff,
        fontSize: 18,
      },
    });
    this.statusText.position.set(20, 20);
    this.container.addChild(this.statusText);

    // Text de missatges
    this.messageText = new Text({
      text: "",
      style: {
        fill: 0xffffff,
        fontSize: 16,
        wordWrap: true,
        wordWrapWidth: 760,
      },
    });
    this.messageText.position.set(20, 60);
    this.container.addChild(this.messageText);
  }

  setStatus(text: string) {
    this.statusText.text = text;
  }

  setMessage(text: string) {
    this.messageText.text = text;
  }
}
