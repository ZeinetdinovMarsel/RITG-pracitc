interface Task{
    id: string;
    title: string;
    comment: string;
    assignedUserId: string;
    creatorId: string;
    priority: number;
    status: number;
    startDate: Date;
    endDate: Date;
}