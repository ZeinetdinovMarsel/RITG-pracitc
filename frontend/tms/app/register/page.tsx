"use client";

import {
  RegisterRequest,
  register
} from "../services/register";
import { useRouter } from "next/navigation";
import UserRegister from "../components/UserRegister";

export default function Register() {

  const router = useRouter();
  const handleRegister = async (request: RegisterRequest) => {
    const success = await register(request);
    if (success) {
      router.push('/login');
    } else {

    }
  }
  const navigateToLogin = () => {
    router.push('/login');
  };
  return (
    <UserRegister
      handleRegister={handleRegister}
      navigateToLogin={navigateToLogin}
    />
  )
}