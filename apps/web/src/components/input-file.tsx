import { useState } from 'react';

type Props = {
  onChangeFile?: (file?: File) => void;
  file?: File | null;
}
export default function InputFile({ onChangeFile, file }: Props) {
  const [state, setState] = useState<'empty' | 'dragging' | 'filled'>('empty'); 
  const handleChangeFiles = async (files: FileList) => {
    onChangeFile?.(files?.[0]);
  }
  return (
    <label 
      htmlFor="file"
      className='w-full h-20 flex items-center justify-center border-2 rounded-lg border-dashed'
    >
      { state === 'empty' && <p>Selecione os arquivos</p> }
      { state === 'dragging' && <p>Solte os arquivos</p> }
      { 
        state === 'filled' && 
        <div>
          { file && <p>{file.name}</p> }
        </div>
      }
      <input
        name='file'
        type="file"
        onDragEnter={() => setState('dragging')}
        onDragLeave={() => setState( file ? 'filled' : 'empty')}
        onDrop={e => {
          e.preventDefault();
          setState('filled');
          handleChangeFiles(e.dataTransfer.files);
        }}
        onChange={e => {
          e.preventDefault();
          if (e.target.files) {
            setState('filled');
            handleChangeFiles(e.target.files);
          }
        }}
        onClick={(e) => {console.log('click')}}
        className='w-full h-20 absolute cursor-pointer opacity-0'
      />
    </label>
  );
}