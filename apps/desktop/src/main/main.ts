import { app, Menu } from 'electron';
import { Application } from './application';

if (process.env.NODE_ENV === 'production') {
  const sourceMapSupport = require('source-map-support');
  sourceMapSupport.install();
}

const isDebug =
  process.env.NODE_ENV === 'development' || process.env.DEBUG_PROD === 'true';

if (isDebug) {
  require('electron-debug')();
}

/**
 * Add event listeners...
 */
const application = new Application();
app.on('window-all-closed', () => {
  if (process.platform === 'darwin') {
    app.dock.hide();
  }
});

app
  .whenReady()
  .then(() => {
    application.createWindow();
    app.on('activate', () => {
      // On macOS it's common to re-create a window in the app when the
      // dock icon is clicked and there are no other windows open.
      if (application.mainWindow === null) application.createWindow();
    });

    application.checkForUpdates();
    var trayContextMenu = Menu.buildFromTemplate([
      {
        label : 'Abrir',
        click: () => {
          application.openOrCreateWindow()
        },
      },
      {
        label: 'Upload arquivos',
        click: () => {
          application.openOrCreateWindow();
          application.events.emit(true, 'upload-files', null);
        },
      },
      {
        label: 'Sair',
        role: 'quit',
        click: () => {
          app.quit()
        }
      },
    ])
    application.addTray('App', undefined, trayContextMenu);
  })
  .catch(console.log);
