const ErrorMessage = ({ message }: { message?: string }) => {
  if (!message) return null;
  return ( 
    <p className="text-red-500 text-xs">{message}</p>  
  );
}
 
export default ErrorMessage;