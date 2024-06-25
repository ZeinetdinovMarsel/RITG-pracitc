interface Task{
    id: string;
    title: string;
    description: string;
    assignedUserId: number;
    priority: string;
    status: string
    startDate: Date;
    endDate: Date;
}