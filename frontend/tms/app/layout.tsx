
import "./globals.css";
import Layout, {Header, Content, Footer} from "antd/es/layout/layout";
import {Menu} from "antd";
import Link from "next/link";

const items = [
  { key: "Home", label: <Link href={"/"}>Home</Link> },
  { key: "Tasks", label: <Link href={"/tasks"}>Tasks</Link> }
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
            Prototype of Task Management System 2024 by Zeinetdinov Marsel
          </Footer>

        </Layout>
      </body>
    </html>
  );
}


