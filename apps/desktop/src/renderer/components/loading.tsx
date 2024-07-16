const Loading = () => {
  return (
    <div className="text-xl text-primary absolute w-full h-full bg-secondary flex flex-col items-center justify-center gap-3">
      <h1>Carregando...</h1>
      <div className="animate-spin w-20 h-20 rounded-full border-4 border-primary border-b-transparent"></div>
    </div>
  );
}
 
export default Loading;