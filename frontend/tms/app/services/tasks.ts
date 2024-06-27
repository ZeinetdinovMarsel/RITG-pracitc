
export interface TaskRequest {
    title: string;
    comment: string;
    assignedUserId: string;
    priority: string;
    status: string
    startDate: Date;
    endDate: Date;
}
export const getAllUsers = async () => {
    const response = await fetch("http://localhost:5183/users",
    );
    await checkResponse(response);
    return response.json();
}

export const getAllTasks = async () => {
    const response = await fetch("http://localhost:5183/tsks",
    );
    await checkResponse(response);
    return response.json();
}

export const createTask = async (taskrequest: TaskRequest) => {

  

    const response = await fetch("http://localhost:5183/tsks", {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        credentials: 'include',
        body: JSON.stringify(taskrequest),
    });
    await checkResponse(response);
}

export const updateTask = async (id: string, taskrequest: TaskRequest) => {
    const response = await fetch(`http://localhost:5183/tsks/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        credentials: 'include',
        body: JSON.stringify(taskrequest),
    });
    await checkResponse(response);
}

export const deleteTask = async (id: string) => {
    const response = await fetch(`http://localhost:5183/tsks/${id}`, {
        method: "DELETE",
        credentials: 'include'
    });
    await checkResponse(response);
}

const checkResponse = async (response: Response) => {
    switch (response.status) {
        case 200:
            return;            
        case 400:
            throw new Error("Неверный запрос.");
        case 401:
            throw new Error("Вы не авторизованы для выполнения этого действия.");
        case 403:
            throw new Error("Доступ запрещен.");
        default:
            throw new Error(`HTTP error! Status: ${response.status}`);
    }
}
