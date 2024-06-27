import { message } from "antd";

export interface LoginRequest {
    email: string;
    password: string;
}

export const login = async (loginRequest: LoginRequest): Promise<boolean> => {
    try {
        const response = await fetch("http://localhost:5183/login", {
            method: "POST",
            headers: {
                "content-type": "application/json",
            },
            body: JSON.stringify(loginRequest),
            credentials: 'include'
        });

        if (response.ok) {
            message.success("Авторизация прошла успешно");
            return true;
        } else {
            const errorResponse = await response.json();
            message.error(`${errorResponse.message}`);
            return false;
        }
    } catch (error) {
        message.error('Произошла ошибка при выполнении запроса');
        return false;
    }
};
