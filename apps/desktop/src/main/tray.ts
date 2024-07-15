import { BrowserWindow, ipcMain, Menu, Tray } from "electron";
export const createTray = (icon: string, window: BrowserWindow) => {
  const tray = new Tray(icon);
  tray.setToolTip("electron-react-boilerplate");
  const contextMenu = Menu.buildFromTemplate([
    { label: 'Item1', type: 'radio', click: () => {
      window.webContents.send('tray-click', ['item1']);
        console.log('item1');
      }
    },
    { label: 'Item2', type: 'radio' },
    { label: 'Item3', type: 'radio', checked: true },
    { label: 'Item4', type: 'radio' }
  ])
  tray.setContextMenu(contextMenu);
  return tray
}
