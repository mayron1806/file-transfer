import { BrowserWindow, ipcMain } from "electron";
export type Channels = 'ipc-example' | 'tray-click' | 'upload-files';

export class EventManager {
  constructor(private mainWindow: BrowserWindow) {}
  addListenner(channel: Channels, func: (...args: unknown[]) => void) {
    if (this.mainWindow.webContents.eventNames().some(e => e === channel)) {
      this.mainWindow.webContents.on(channel as any, func);
    } else {
      ipcMain.on(channel, func);
    }
  }
  emit(webContents: boolean = true, channel: Channels, ...args: unknown[]) {
    if(this.mainWindow.isDestroyed()) return;

    if (webContents) {
      this.mainWindow.webContents?.send(channel, ...args);
    } else {
      ipcMain.emit(channel, ...args);
    }
  }
}
