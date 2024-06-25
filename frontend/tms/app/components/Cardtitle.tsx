
interface Props{
    title: string;
    description: string;
    assignedUserId: number;
    priority: string;
    status: string
    startDate: Date;
    endDate: Date;
}
export const CardTitle = ({
    title,
    description,
    assignedUserId,
    priority,
    status,
    startDate,
    endDate
}:Props)=>{
    return(
        <div style={{
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            justifyContent: "space-between",

        }}>
            <p className="card_title">Задача:</p>
            <p className="card_title">{title}</p>
            <p className="card_price">{description}</p>
            <p className="card_price">{assignedUserId}</p>
            <p className="card_price">{priority}</p>
            <p className="card_price">{status}</p>
        </div>
    )
}