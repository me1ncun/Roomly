import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useContext } from 'react';
import { AuthContext } from '../../store/AuthContext';

export const RequireAuth = () => {
  const { authorized } = useContext(AuthContext);
  const { pathname } = useLocation();

  if (!authorized) {
    return <Navigate to="/login" state={{ pathname }} replace />;
  }

  return <Outlet />;
};