import { BrowserWindow, Menu, shell } from "electron";
import MenuBuilder from "./menu";
import { EventManager } from "./event";
import { TrayBuilder } from "./tray";

declare const MAIN_WINDOW_WEBPACK_ENTRY: string;
declare const MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY: string;

export class Application {
  events: EventManager;
  mainWindow: BrowserWindow;
  tray: TrayBuilder;
  constructor() {}
  addTray(name?: string, menu?: Menu) {
    this.tray = new TrayBuilder();
    this.tray.init(name);
    this.tray.setContextMenu(menu);
  }
  async openOrCreateWindow() {
    if (this.mainWindow) {
      if (this.mainWindow.isMinimized()) {
        this.mainWindow.restore();
      }
      this.mainWindow.focus();
    } else {
      await this.createWindow();
    }
  }
  async createWindow(
    options?: Electron.BrowserWindowConstructorOptions
  ) {
    this.mainWindow = new BrowserWindow({
      show: false,
      width: 1024,
      height: 728,
      webPreferences: {
        preload: MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY
      },
      ...options,
    });
    this.mainWindow.loadURL(MAIN_WINDOW_WEBPACK_ENTRY);
    this.mainWindow.on('ready-to-show', () => {
      if (!this.mainWindow) {
        throw new Error('"mainWindow" is not defined');
      }
      this.mainWindow.show();
    });
    this.mainWindow.on('closed', () => {
      this.mainWindow = undefined;
    });
    const menuBuilder = new MenuBuilder(this.mainWindow);
    menuBuilder.buildMenu();
    this.mainWindow.webContents.setWindowOpenHandler((edata) => {
      shell.openExternal(edata.url);
      return { action: 'deny' };
    });

    this.events = new EventManager(this.mainWindow);
  }
}
