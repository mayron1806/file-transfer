import { app, BrowserWindow, Menu, shell } from "electron";
import path from "path";
import { autoUpdater } from 'electron-updater';
import log from 'electron-log';
import { resolveHtmlPath } from "./util";
import MenuBuilder from "./menu";
import { EventManager } from "./event";
import { TrayBuilder } from "./tray";

class AppUpdater {
  constructor() {
    log.transports.file.level = 'info';
    autoUpdater.logger = log;
  }
  checkForUpdatesAndNotify() {
    autoUpdater.checkForUpdatesAndNotify();
  }
}
export class Application {
  events: EventManager;
  mainWindow: BrowserWindow;
  tray: TrayBuilder;
  private isDebug: boolean = process.env.NODE_ENV === 'development' || process.env.DEBUG_PROD === 'true';
  private updater: AppUpdater = new AppUpdater();
  constructor() {}
  addTray(name?: string, iconAssetPath?: string, menu?: Menu) {
    this.tray = new TrayBuilder(this.getAssetPath(...iconAssetPath?.split('/') ?? ['icon.png']));
    this.tray.init(name);
    this.tray.setContextMenu(menu);
  }
  checkForUpdates() {
    this.updater = new AppUpdater();
    this.updater.checkForUpdatesAndNotify();
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
    if (this.isDebug) {
      await this.installExtensions();
    }
    const icon = this.getAssetPath('icon.png');
    this.mainWindow = new BrowserWindow({
      show: false,
      width: 1024,
      autoHideMenuBar: true,
      height: 728,
      icon,
      webPreferences: {
        preload: app.isPackaged
          ? path.join(__dirname, 'preload.js')
          : path.join(__dirname, '../../.erb/dll/preload.js'),
      },
      ...options,
    });
    this.mainWindow.loadURL(resolveHtmlPath('index.html'));
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
  private installExtensions() {
    const installer = require('electron-devtools-installer');
    const forceDownload = !!process.env.UPGRADE_EXTENSIONS;
    const extensions = ['REACT_DEVELOPER_TOOLS'];

    return installer
      .default(
        extensions.map((name) => installer[name]),
        forceDownload,
      )
      .catch(console.log);
  }
  private getAssetPath(...paths: string[]): string {
    const RESOURCES_PATH = app.isPackaged
    ? path.join(process.resourcesPath, 'assets')
    : path.join(__dirname, '../../assets');
    return path.join(RESOURCES_PATH, ...paths);
  };
}
