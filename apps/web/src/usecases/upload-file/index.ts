type Result = {
  start: () => Promise<void>;
  abort: () => Promise<void>;
}
export const uploadFile = (
  url: string, 
  file: File, 
  onProgress?: (progress: number) => void,
  onEnd?: (success?: boolean) => void,
): Result => {
  const xhr = new XMLHttpRequest();
  
  xhr.upload.onprogress = (e) => {
    if (e.lengthComputable) {
      onProgress?.((e.loaded / e.total) * 100);
    }
  };

  xhr.upload.onerror = () => {
    onEnd?.(false);
    throw new Error("Erro ao fazer upload.");
  };

  xhr.upload.onabort = () => {
    onEnd?.(false);
    throw new Error("Upload cancelado.");
  };

  xhr.onload = () => {
    if (xhr.status === 200) onEnd?.(true);
    else {
      onEnd?.(false);
      throw new Error("Erro ao fazer upload.");
    }
  };

  xhr.open("PUT", url, true);
  return {
    abort: async () => xhr.abort(),
    start: async () => {
      xhr.send(file);
    },
  };
};
