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

export const Root = () => (
  <Provider store={store}>
    <Router>
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<HomePage />} />
          <Route path="rooms">
            <Route path=":currentRoom?" element={<RoomsPage />} />
          </Route>
          <Route path="*" element={<NotFoundPage />} />
          <Route path="home" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </Router>
  </Provider>
);
