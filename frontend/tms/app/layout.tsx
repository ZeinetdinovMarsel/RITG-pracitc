
import "./globals.css";
import Layout, {Header, Content, Footer} from "antd/es/layout/layout";
import {Menu} from "antd";
import Link from "next/link";

const items = [
  { key: "Home", label: <Link href={"/"}>Главная</Link> },
  { key: "Tasks", label: <Link href={"/tasks"}>Задачи</Link> },
  { key: "Auth", label: <Link href={"/auth"}>Авторизация/Регистрация</Link> }
];

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Layout style={{ minHeight: "100vh" }}>
          <Header>
            <Menu
              theme="dark"
              mode="horizontal"
              items={items}
              style={{ flex: 1, minWidth: 0 }}
            />
          </Header>
          <Content style={{ padding: "0 48px" }}>{children}</Content>
          <Footer style={{ textAlign: "center" }}>
            Прототип Task Management System 2024 Зейнетдинова Марселя
          </Footer>

        </Layout>
      </body>
    </html>
  );
}


