import { Suspense } from "react";
import ActiveAccount from "./active-account";

const ActiveAccountPage = ({ searchParams }: { searchParams: { token: string }}) => {
  return ( 
    <Suspense fallback={<div>Loading...</div>}>
      <ActiveAccount token={searchParams.token} />
    </Suspense>
  );
}
 
export default ActiveAccountPage;