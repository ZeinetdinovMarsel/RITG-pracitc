
export interface TaskRequest {
    title: string;
    comment: string;
    assignedUserId: string;
    priority: number;
    status: string;
    startDate: Date;
    endDate: Date;
}
export const getUsersbyRole = async (roleId) => {
    const url = `http://localhost:5183/users?roleId=${roleId}`;

    const response = await fetch(url, {
        method: 'GET',
        credentials: 'include',
    });

    if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return response.json();
}

export const getAllTasks = async () => {
    const response = await fetch("http://localhost:5183/tsks", {
        credentials: 'include'
    },
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

export const changeStateTask = async (id: string) => {
    const response = await fetch(`http://localhost:5183/tsks/status/change/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        credentials: 'include',
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

export const getTaskHistoryById = async (id: string) => {
    const url = `http://localhost:5183/tsks/history?id=${id}`;

    const response = await fetch(url, {
        method: 'GET',
        credentials: 'include',
    });

    await checkResponse(response);
    
    return response.json();
};

const checkResponse = async (response: Response) => {

    switch (response.status) {
        case 200:
            return;
        case 400:
            throw new Error(`${await response.json()}`);
        case 401:
            throw new Error("Вы не авторизованы для выполнения этого действия.");
        case 403:
            throw new Error("Доступ запрещен.");
        default:
            throw new Error(`HTTP error! Status: ${response.status}`);
    }
}
