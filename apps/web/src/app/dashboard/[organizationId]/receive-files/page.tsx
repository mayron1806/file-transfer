import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import ReceiveFilesTable from "./components/receive-files-table";
import NewReceiveFilesDialog from "@/components/dialogs/new-receive-files-transfer";

const ReceiveFilesPage = () => {
  return ( 
    <Card x-chunk="dashboard-06-chunk-0">
      <CardHeader className="flex flex-row justify-between">
        <div>
          <CardTitle>Arquivos a receber</CardTitle>
          <CardDescription>
            Aqui ficam os arquivos que foram recebidos
          </CardDescription>
        </div>
        <NewReceiveFilesDialog />
      </CardHeader>
      <CardContent>
        <ReceiveFilesTable />
      </CardContent>
    </Card>
  );
}
 
export default ReceiveFilesPage;