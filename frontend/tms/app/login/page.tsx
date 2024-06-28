"use client";

import { FormEvent, useEffect, useState } from "react";
import {
  LoginRequest,
  login
} from "../services/login";
import { UserLogin } from "../components/UserLogin";
import { useRouter } from "next/navigation";

export default function Login() {
  const defaultValues = {
    email: "",
    password: ""
  } as Login


  const [values, setValues] = useState<Login>(defaultValues);
  const router = useRouter();
  const handleLogin = async (request: LoginRequest) => {
    const success = await login(request);
    if (success) {
      router.push('/');
      window.location.reload();
    } else {

    }
  }
  const navigateToRegister = () => {
    router.push('/register');
  };
  return (
    <UserLogin
      values={values}
      handleLogin={handleLogin}
      navigateToRegister={navigateToRegister}
    />
  )
}