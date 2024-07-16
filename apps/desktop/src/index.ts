import { app, Menu } from 'electron';
import { Application } from './application/application';
// Handle creating/removing shortcuts on Windows when installing/uninstalling.
if (require('electron-squirrel-startup')) {
  app.quit();
}

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
    application.addTray('App', trayContextMenu);
  })
  .catch(console.log);