import * as React from "react";
import { CheckIcon, ChevronsUpDown, X } from "lucide-react";
import {
  Command,
  CommandGroup,
  CommandItem,
  CommandList,
  CommandInput,
  CommandEmpty
} from "@/components/ui/command";
import { useRef, useState } from "react";
import { Popover, PopoverContent, PopoverTrigger } from "./ui/popover";
import { Button } from "./ui/button";
import { cn } from "@/lib/utils";
import { allowedAudio, allowedCompressedFiles, allowedVideo, allowedDocuments, allowedImages, allowedOthers } from "@/constraints/allowed-files";

const allowedTypes = {
  'Imagens': allowedImages,
  'Vídeos': allowedVideo,
  'Documentos': allowedDocuments,
  'Arquivos comprimidos': allowedCompressedFiles,
  'Aúdio': allowedAudio,
  'Outros': allowedOthers
}

type Props = {
  error?: string;
  value?: string[];
  onAdd?: (value: string) => void;
  onRemove?: (index: number) => void;
  placeholder?: string;
  className?: string;
};
const formatFileExt = (ext: string) => {
  return ext.replace('.', '').toUpperCase();
}
export default function SelectFileTypeList({
  error,
  className,
  value,
  onAdd,
  onRemove,
  placeholder,
}: Props) {
  const inputRef = useRef<HTMLInputElement>(null);
  const [open, setOpen] = useState(false);
  const [inputValue, setInputValue] = useState<string>("");
  return (
    <div className={cn("flex flex-col gap-2", className)}>
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            variant="outline"
            role="combobox"
            aria-expanded={open}
            className="w-full justify-between text-wrap text-left h-auto"
          >
            {(!value || value?.length === 0) && <p>Selecione os arquivos permitidos</p>}
            {
              value &&
              <div className="inline-flex gap-1 flex-wrap">
                {value?.length > 0 && value?.map((item, index) => (
                  <p key={item} className="flex items-center gap-2 py-1 px-2 bg-card">
                    {formatFileExt(item)}
                    <X
                      onClick={(e) => {
                        e.stopPropagation();
                        onRemove?.(index);
                      }}
                      className="h-4 w-4 hover:bg-primary p-0.5 text-destructive rounded-md hover:text-secondary"
                      strokeWidth={4} 
                    />
                  </p>
                ))}
              </div>
            }
            <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
          </Button>
        </PopoverTrigger>
        <PopoverContent arrowsWheel className="w-full p-0">
          <Command>
            <CommandInput
              ref={inputRef}
              value={inputValue}
              onValueChange={setInputValue}
              placeholder={placeholder}
              className="ml-2 flex-1 bg-transparent outline-none placeholder:text-muted-foreground"
            />
            <CommandList>
              <CommandEmpty>Adicione e-mails</CommandEmpty>
                {
                  Object.entries(allowedTypes).map(([key, allowedFiles]) => (
                    <CommandGroup key={key} heading={key}>
                      {
                        allowedFiles.map((item) => (
                            <CommandItem
                              key={item}
                              value={item}
                              onSelect={(currentValue) => {
                                if (value?.includes(item)) {
                                  setInputValue('');
                                  onRemove?.(value.findIndex((value) => value === item));
                                } else {
                                  setInputValue('');
                                  onAdd?.(currentValue);
                                }
                              }}
                            >
                              {formatFileExt(item)}
                              <CheckIcon
                                className={cn(
                                  "ml-auto h-4 w-4",
                                  value?.includes(item) ? "opacity-100" : "opacity-0"
                                )}
                              />
                            </CommandItem>
                          )
                        )
                      }
                    </CommandGroup>
                  ))
                }
            </CommandList>
          </Command>
        </PopoverContent>
      </Popover>
      {
        error && <p className="text-sm text-destructive">{error}</p>
      }
    </div>
  );
}
