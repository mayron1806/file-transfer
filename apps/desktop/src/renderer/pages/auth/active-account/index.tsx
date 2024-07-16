import { Controls, Player } from '@lottiefiles/react-lottie-player';
const ActiveAccount = () => {
  const success = true;
  if (success) return <SuccessActiveAccount />;
  return <ErrorActiveAccount />;
}
const SuccessActiveAccount = () => {
  return ( 
    <div>
      
    </div>
  );
}
const ErrorActiveAccount = () => {
  return ( 
    <div>
      {/* <Loading /> */}
    </div>
  );
}
export default ActiveAccount;