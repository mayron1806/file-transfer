'use client';
import { forwardRef, useState, InputHTMLAttributes, useEffect } from 'react';
import { Folder, FolderOpen } from 'lucide-react';

type Props = {
  files?: FileList;
  allowedExtensions?: string[];
} & InputHTMLAttributes<HTMLInputElement>;

const InputFile = forwardRef<HTMLInputElement, Props>(({ files, allowedExtensions, multiple, ...props }, ref) => {
  const [state, setState] = useState<'empty' | 'dragging' | 'filled'>('empty');
  
  const fileNamesToDisplay = Object.values(files ?? []).slice(0, 3).map(file => file.name);
  const additionalFileCount = (files?.length ?? 0) - 3;
  
  useEffect(() => {
    if ((files?.length ?? 0) > 0) setState('filled');
    else setState('empty');
  }, [files]);

  return (
    <label
      htmlFor="file"
      className='w-full min-h-40 flex flex-col items-center justify-center gap-4 border-2 rounded-lg border-dashed overflow-hidden relative'
    >
      { state === 'empty' && <p>Selecione os arquivos</p> }
      { state === 'dragging' && <p>Solte os arquivos</p> }
      { 
        state === 'filled' && 
        <div className="text-center text-sm font-bold">
          { fileNamesToDisplay.map(fileName => <p key={fileName}>{fileName}</p>) }
          { additionalFileCount > 0 && ` e mais ${additionalFileCount} arquivos` }
        </div>
      }
      { state === 'empty' && <Folder className="w-8 h-8" strokeWidth={1} /> }
      { state === 'dragging' && <FolderOpen className="w-8 h-8" strokeWidth={1} /> }
      <input
        ref={ref}
        name='file'
        type="file"
        multiple={multiple}
        onDragEnter={() => setState('dragging')}
        onDragLeave={() => setState(props.value ? 'filled' : 'empty')}
        onDrop={() =>  setState('filled')}
        className='w-full h-full absolute cursor-pointer opacity-0'
        accept={allowedExtensions?.join(',')}
        {...props}
      />
    </label>
  );
});

export default InputFile;
