/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
const BASE_URL = 'http://localhost:7001/api/users';

// a promise resolved after a given delay
function wait(delay: number) {
  return new Promise(resolve => {
    setTimeout(resolve, delay);
  });
}

// To have autocompletion and avoid mistypes
type RequestMethod = 'GET' | 'POST' | 'PATCH' | 'DELETE';

function request<T>(
  url: string,
  method: RequestMethod = 'GET',
  data: any = null, // we can send any data to the server
): Promise<T> {
  const options: RequestInit = { method };

  if (data) {
    // We add body and Content-Type only for the requests with data
    options.body = JSON.stringify(data);
    options.headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
  }


  // return wait(300)
  // .then(() => fetch(BASE_URL + url, options))
  // .then(response => response.json());

  // for a demo purpose we emulate a delay to see if Loaders work
  return wait(300)
    .then(() => fetch(BASE_URL + url, options))
    .then(async (response) => {
      const text = await response.text();
      console.log("Server Response:", text);

      try {
        const parsed = JSON.parse(text);
        
        if (!response.ok) {
          console.error("Request error:", parsed);
          throw new Error(JSON.stringify(parsed));
        }

        return parsed;
      } catch (error: any) {
        if (!response.ok) {
          throw new Error(text || "Serv error");
        }
        return text as unknown as T;
      }
    });
}

export const client = {
  get: <T>(url: string) => request<T>(url),
  post: <T>(url: string, data?: any) => request<T>(url, 'POST', data),
  patch: <T>(url: string, data: any) => request<T>(url, 'PATCH', data),
  delete: (url: string) => request(url, 'DELETE'),
};
