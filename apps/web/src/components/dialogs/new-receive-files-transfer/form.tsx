'use client';

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Calendar } from "lucide-react";
import { Textarea } from "../../ui/textarea";
import { useOrganization } from "@/context/organization-context";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../../ui/form";
import { ReceiveFilesSchema, receiveFilesSchema } from "@/validation/receive-files";
import moment from "moment";
import { DatePicker } from "@/components/date-picker";
import SelectFileTypeList from "@/components/select-file-type-list";

type Props = {
  onSubmit: (body: ReceiveFilesSchema) => Promise<void>;
}
const ReceiveFilesForm = ({ onSubmit }: Props) => {
  const { organization } = useOrganization();
  const limits = organization.plan.limits;
  const form = useForm<ReceiveFilesSchema>({
    resolver: zodResolver(receiveFilesSchema),
    defaultValues: {
      name: '',
      message: '',
      password: '',
      maxFiles: limits.maxUploadConcurrency,
    }
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
                  <Input id="name" {...field} type="text" autoComplete="name" placeholder="Ex.: Arquivos de audio" />
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
                    <Input id="password" autoComplete="new-password" type="password" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          )
        }
        {
          <FormField 
            control={form.control}
            name="acceptedFiles"
            render={({ field }) => (
              <FormItem>
                <FormLabel htmlFor="acceptedFiles">Tipos de arquivos aceitos</FormLabel>
                <FormControl>
                  <SelectFileTypeList
                    onAdd={newFile => field.onChange([...field.value ?? [], newFile])}
                    value={field.value}
                    onRemove={index => field.onChange(field.value?.filter((_, i) => i !== index))}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        }
        {
          limits.maxUploadConcurrency > 1 && (
            <FormField
              control={form.control}
              name="maxFiles"
              render={({ field }) => (
                <FormItem>
                  <FormLabel htmlFor="maxFiles">Quantidade de arquivos</FormLabel>
                  <FormControl>
                    <Input 
                      id="maxFiles" 
                      autoComplete="off" 
                      type="number" 
                      {...field} 
                      onChange={event => {
                        const value = Number(event.target.value);
                        field.onChange(value > limits.maxUploadConcurrency ? limits.maxUploadConcurrency : value);
                      }} 
                    />
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
        <Button 
          type="submit"
          disabled={form.formState.isSubmitting}
          loading={form.formState.isSubmitting}
        >Criar</Button>
      </form>
    </Form>
  );
}
 
export default ReceiveFilesForm;