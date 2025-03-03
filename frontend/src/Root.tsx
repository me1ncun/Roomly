import { Navigate, Route, BrowserRouter as Router, Routes } from "react-router-dom";
import { App } from "./App";
import { HomePage } from "./components/HomePage";
import { RoomsPage } from "./components/RoomsPage";
import { NotFoundPage } from "./components/NotFoundPage";
import { LoginPage } from "./components/LoginPage/LoginPage";
import { RegisterPage } from "./components/RegisterPage/RegisterPage";
import { Provider } from "react-redux";
import { store } from "./app/store";

export const Root = () => (
  <Provider store={store}>
    <Router>
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<HomePage />} />
          <Route path="rooms" element={<RoomsPage />} />
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
        </Route>
        <Route path="*" element={<NotFoundPage />} />
        <Route path="home" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  </Provider>
);
