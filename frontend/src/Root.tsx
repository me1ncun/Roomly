import {
  Navigate,
  Route,
  HashRouter as Router,
  Routes,
} from "react-router-dom";
import { App } from "./App";
import { HomePage } from "./components/HomePage";
import { PeoplePage } from "./components/PeoplePage";
import { NotFoundPage } from "./components/NotFoundPage";
// import { Provider } from "react-redux";
// import { store } from "./app/store";

export const Root = () => (
  // <Provider store={store}>
    <Router>
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<HomePage />} />
          <Route path="people">
            <Route path=":currentPerson?" element={<PeoplePage />} />
          </Route>
          <Route path="*" element={<NotFoundPage />} />
          <Route path="home" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </Router>
  // </Provider>
);
