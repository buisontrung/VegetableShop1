import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';

import { GoogleOAuthProvider } from '@react-oauth/google';

// Thay YOUR_GOOGLE_CLIENT_ID bằng clientId của bạn từ Google Developer Console


ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <GoogleOAuthProvider clientId="351636607233-d0cbhf3btn0mursiue86j09io0b0lka4.apps.googleusercontent.com">
      <App />
    </GoogleOAuthProvider>
  </React.StrictMode>,
);
