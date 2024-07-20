import NewSendFilesDialog from "@/components/dialogs/new-send-files-transfer";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";

const HomePage = () => {
  // get files
  return ( 
    <Card x-chunk="dashboard-06-chunk-0">
      <CardHeader className="flex flex-row justify-between">
        <div>
          <CardTitle>Arquivos enviados</CardTitle>
          <CardDescription>
            Aqui ficam os arquivos enviados
          </CardDescription>
        </div>
        <NewSendFilesDialog />
      </CardHeader>
      <CardContent>
      </CardContent>
    </Card>
  );
}
 
export default HomePage;