'use client';
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Separator } from "@/components/ui/separator";
import { X } from "lucide-react";
import { Label, Pie, PieChart } from "recharts";
import {
  ChartConfig,
  ChartContainer,
  ChartLegend,
  ChartLegendContent,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { formatBytes } from "@/lib/format-bytes";
const chartConfig = {
  uploading: { label: "Enviando" },
  success: { label: "Enviado" },
  error: { label: "Erro" }
} satisfies ChartConfig;
export type UploadProgress = {
  fileName: string;
  progress: number;
  finished: boolean;
  error?: string;
  size: number;
  abort?: () => void;
}
type Props = {
  uploadProgress: Array<UploadProgress>;
}
const SendingFiles = ({ uploadProgress: uploads }: Props) => {
  const uploading = uploads.filter(item => item.progress < 100 && !item.finished);
  const uploadsWithError = uploads.filter(item => item.error);
  const totalFiles = uploads.length;
  return (
    <div className="space-y-4">
      <div>
        <ChartContainer
          config={chartConfig}
          className="mx-auto aspect-square max-h-[250px]"
        >
          <PieChart>
            <ChartTooltip
              content={<ChartTooltipContent hideLabel />}
            />
            <Pie
              data={[
                { state: 'error', amount: uploadsWithError.length, fill: "hsl(var(--chart-1))", },
                { state: 'uploading', amount: uploading.length, fill: "hsl(var(--chart-2))", },
                { state: 'success', amount: totalFiles - uploadsWithError.length - uploading.length, fill: "hsl(var(--chart-3))" }
              ]}
              dataKey="amount"
              nameKey="state"
              innerRadius={60}
            >
              <Label
                content={({ viewBox }) => {
                  if (viewBox && "cx" in viewBox && "cy" in viewBox) {
                    return (
                      <text
                        x={viewBox.cx}
                        y={viewBox.cy}
                        textAnchor="middle"
                        dominantBaseline="middle"
                      >
                        <tspan
                          x={viewBox.cx}
                          y={viewBox.cy}
                          className="fill-foreground text-3xl font-bold"
                        >
                          {totalFiles.toLocaleString()}
                        </tspan>
                        <tspan
                          x={viewBox.cx}
                          y={(viewBox.cy || 0) + 24}
                          className="fill-muted-foreground"
                        >
                          Arquivos
                        </tspan>
                      </text>
                    )
                  }
                }}
              />
            </Pie>
            <ChartLegend content={<ChartLegendContent />} />
          </PieChart>
        </ChartContainer>
      </div>
      <Separator />
      {
        uploading.length > 0 && (
          <div className="flex flex-col gap-2">
            <h3 className="text-xl font-bold">Uploads em andamento</h3>
            {
              uploading.map(({ fileName, progress, abort, size }) => (
                <div key={fileName} className="p-1 px-2 border relative rounded">
                  <p className="text-sm font-medium">{fileName}</p>
                  <span className="text-xs">{Math.ceil(progress)}%</span> - <span className="text-xs">{formatBytes(size)}</span>
                  <div className="w-full bg-muted rounded-full h-1">
                    <div className="bg-primary h-full rounded-full" style={{ width: `${progress}%` }}></div>
                  </div>
                  {
                    abort && (
                      <Button onClick={abort} className="absolute p-1 top-2 right-2 w-auto h-auto" variant="ghost" size="icon">
                        <X className="h-4 w-4"/>
                      </Button>
                    )
                  }
                </div>
              ))
            }
          <Separator />
          </div>
        )
      }
      {
        uploadsWithError.length > 0 && (    
          <div>
            <h3 className="text-xl font-bold">Uploads com erro</h3>
            <p className="text-sm">{uploadsWithError.length} arquivos com erros</p>
            <ScrollArea>
              <div className="mt-2 flex flex-col gap-2 h-64 pr-2.5">
                {
                  uploadsWithError.map(({ fileName, error }) => (
                    <div key={fileName} className="p-1 px-2 border relative rounded">
                      <p className="text-sm font-medium">{fileName}</p>
                      <span className="text-xs font-medium text-destructive">Erro: {error}</span>
                      <div className="w-full bg-muted rounded-full h-1" />
                    </div>
                  ))
                }
              </div>
            </ScrollArea>
          </div>
        )
      }
    </div>
  );
}
export default SendingFiles;