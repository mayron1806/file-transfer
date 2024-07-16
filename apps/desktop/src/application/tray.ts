import { app, Menu, nativeImage, Tray } from "electron";

export class TrayBuilder {
  private tray: Tray;
  private iconPath: string = '/assets/icon.png';
  constructor() {
    
    const path = `${app.getAppPath()}${this.iconPath}`;
    const image = nativeImage.createFromPath(path);
    this.tray = new Tray(image);
  }
  init(tooltip: string) {
    this.tray.setToolTip(tooltip);
  }
  setContextMenu(contextMenu: Menu) {
    this.tray.setContextMenu(contextMenu);
  }
}
