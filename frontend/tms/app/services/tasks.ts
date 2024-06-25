export interface TaskRequest {
    title: string;
    description: string;
    assignedUserId: number;
    priority: string;
    status: string
    startDate: Date;
    endDate: Date;
}

export const getAllTasks = async () => {
    const response = await fetch("http://localhost:5183/Tsks")
    return response.json();
}

export const createTask = async (taskrequest: TaskRequest) => {
    await fetch("http://localhost:5183/Tsks", {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(taskrequest),
    });
}

export const updateTask = async (id: string, taskrequest: TaskRequest) => {
    await fetch(`http://localhost:5183/Tsks/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(taskrequest),
    });
}

export const deleteTask = async (id: string) => {
    await fetch(`http://localhost:5183/Tsks/${id}`, {
        method: "DELETE",
    });
}