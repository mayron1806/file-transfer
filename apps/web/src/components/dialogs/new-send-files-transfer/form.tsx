'use client';

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Calendar } from "lucide-react";
import InputFile from "../../input-file";
import { Textarea } from "../../ui/textarea";
import SelectEmailList from "../../select-email-list";
import { DatePicker } from "../../date-picker";
import { useOrganization } from "@/context/organization-context";
import { Switch } from "../../ui/switch";
import moment from "moment";
import { allowedFiles } from "@/constraints/allowed-files";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { SendFilesSchema, sendFilesSchema } from "@/validation/send-files";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../../ui/form";

type Props = {
  onSubmit: (body: SendFilesSchema) => Promise<void>;
}
const SendFilesForm = ({ onSubmit }: Props) => {
  const { organization } = useOrganization();
  const limits = organization.plan.limits;
  const form = useForm<SendFilesSchema>({
    resolver: zodResolver(sendFilesSchema),
  });
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} autoComplete="off" className="grid gap-4 py-4 p-1">
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => {
            return (
              <FormItem>
                <FormLabel tooltip="Nome para a transferência de arquivos" htmlFor="name" className="text-right">Nome</FormLabel>
                <FormControl>
                  <Input id="name" {...field} autoComplete="off" placeholder="Ex.: Arquivos de audio" />
                </FormControl>
                <FormMessage />
              </FormItem>
            );
          }}
        />
        <FormField
          control={form.control}
          name="files"
          render={({ field }) => {
            return (
              <FormItem>
                <FormControl>
                  <InputFile
                    multiple={limits.maxUploadConcurrency > 1}
                    allowedExtensions={allowedFiles}
                    files={field.value}
                    {...form.register('files')}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            );
          }}
        />
        <FormField
          control={form.control}
          name="message"
          render={({ field }) => {
            return (
              <FormItem>
                <FormLabel htmlFor="message" className="text-right">Mensagem</FormLabel>
                <FormControl>
                  <Textarea
                    id="message"
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            );
          }}
        />
        {
          limits.canUsePassword && (
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor="password">Senha</FormLabel>
                  <FormControl>
                    <Input id="password" autoComplete="off" type="password" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )
        }
        <FormField
          control={form.control}
          name="expiresAt"
          render={({ field }) => (
            <FormItem>
              <FormLabel htmlFor="expiresAt">Data de expiração</FormLabel>
              <FormControl>
                <DatePicker
                  selected={field.value} 
                  onSelect={field.onChange}
                  disabled={{ 
                    before: moment().add(1, 'day').toDate(),
                    after: moment().add(limits.maxExpireDays, 'day').toDate(),
                  }}
                  mode="single"
                >
                  <Calendar className="mr-2 h-4 w-4" />
                  {field.value ? (
                    moment(field.value).format("DD/MM/YYYY")
                  ) : (
                    <span>Escolha uma data</span>
                  )}
                </DatePicker>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {
          limits.canUseExpiresOnDownload && (
            <FormField
              control={form.control}
              name="expiresOnDownload"
              render={({ field }) => (
                <FormItem className="flex flex-col">
                  <FormLabel htmlFor="expiresOnDownload">Expirar ao fazer download</FormLabel>
                  <FormControl>
                    <Switch checked={field.value} onCheckedChange={field.onChange} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )
        }
        {
          limits.maxEmails > 1 && (
            <FormField 
              name="emailsDestination"
              control={form.control}
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor="emails">Emails</FormLabel>
                  <FormControl>
                    <SelectEmailList 
                      onAdd={newEmail => field.onChange([...field.value ?? [], newEmail])} 
                      value={field.value}
                      onRemove={index => field.onChange(field.value?.filter((email, i) => i !== index))}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )
        }
        <Button 
          type="submit"
          disabled={form.formState.isSubmitting}
          loading={form.formState.isSubmitting}
        >Enviar</Button>
      </form>
    </Form>
  );
}
 
export default SendFilesForm;