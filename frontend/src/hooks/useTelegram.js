import { createContext, useContext, useEffect, useState } from 'react';

const TelegramContext = createContext({});

export const TelegramProvider = ({ children }) => {
  const [tg, setTg] = useState(null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const telegram = window.Telegram?.WebApp;
    if (telegram) {
      telegram.ready();
      telegram.expand();
      setTg(telegram);
      setUser(telegram.initDataUnsafe?.user);
    }
  }, []);

  const value = {
    tg,
    user,
    sendData: (data) => {
      if (tg) {
        tg.sendData(JSON.stringify(data));
      }
    },
    closeApp: () => {
      if (tg) {
        tg.close();
      }
    }
  };

  return (
    <TelegramContext.Provider value={value}>
      {children}
    </TelegramContext.Provider>
  );
};

export const useTelegram = () => {
  const context = useContext(TelegramContext);
  if (!context) {
    throw new Error('ошибка');
  }
  return context;
};
