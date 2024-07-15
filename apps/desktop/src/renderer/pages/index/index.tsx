import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const Index = () => {
  const navigate = useNavigate();
  useEffect(() => {
    navigate('/signin');
  }, [])
  return (
    <div className="text-xl">
      Index
    </div>
  );
}

export default Index;
