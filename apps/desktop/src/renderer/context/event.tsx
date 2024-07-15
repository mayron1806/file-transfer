import { createContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const EventManager = createContext({});

const EventProvider = ({ children }: { children: React.ReactNode }) => {
  const navigate = useNavigate();

  useEffect(() => {
    const uploadFiles = () => navigate('/upload-files');

    const unsubscribe = window.electron.ipcRenderer.on('upload-files', uploadFiles);
    return () => {
      unsubscribe();
    }
  }, [])
  return (
    <EventManager.Provider value={{}}>
      {children}
    </EventManager.Provider>
  );
}

export default EventProvider;
