import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAppSelector } from "../../app/hooks";

export const RequireAuth = () => {
  const authorized = useAppSelector((state) => state.auth.authorized);
  const { pathname } = useLocation();

  if (!authorized) {
    return <Navigate to="/login" state={{ pathname }} replace />;
  }

  return <Outlet />;
};
