import { Menu, Tray } from "electron";
export const createTray = (icon: string) => {
  const tray = new Tray(icon);
  tray.setToolTip("electron-react-boilerplate");
  const contextMenu = Menu.buildFromTemplate([
    { label: 'Item1', type: 'radio' },
    { label: 'Item2', type: 'radio', },
    { label: 'Item3', type: 'radio', checked: true },
    { label: 'Item4', type: 'radio' }
  ])
  tray.setContextMenu(contextMenu);
  return tray
}
export class TrayBuilder {
  private tray: Tray;
  constructor(icon: string) {
    this.tray = new Tray(icon);
  }
  init(tooltip: string) {
    this.tray.setToolTip(tooltip);
  }
  setContextMenu(contextMenu: Menu) {
    this.tray.setContextMenu(contextMenu);
  }
}
