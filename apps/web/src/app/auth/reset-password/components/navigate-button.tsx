'use client'
import { Button } from "@/components/ui/button";
import { useRouter } from "next/navigation";

const NavigateButton = () => {
  const router = useRouter();
  return ( 
    <Button onClick={() => router.push('/auth/login')}>Ir para login</Button>
  );
}
 
export default NavigateButton;