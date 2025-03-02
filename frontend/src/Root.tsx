import {
  Navigate,
  Route,
  HashRouter as Router,
  Routes,
} from "react-router-dom";
import { App } from "./App";
import { HomePage } from "./components/HomePage";
import { RoomsPage } from "./components/RoomsPage";
import { NotFoundPage } from "./components/NotFoundPage";
import { Provider } from "react-redux";
import { store } from "./app/store";
import { AuthProvider } from "./store/AuthContext";
import { LoginPage } from "./components/LoginPage";
import { RequireAuth } from "./components/RequireAuth";

export const Root = () => (
  <Provider store={store}>
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<HomePage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="rooms" element={<RequireAuth />}>
              <Route index element={<RoomsPage />} />
            </Route>
            <Route path="*" element={<NotFoundPage />} />
            <Route path="home" element={<Navigate to="/" replace />} />
          </Route>
        </Routes>
      </Router>
    </AuthProvider>
  </Provider>
);
