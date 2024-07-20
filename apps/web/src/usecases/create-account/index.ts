type Params = {
  name: string;
  email: string;
  password: string;
}
export const createAccount = async (params: Params) => {
  const res = await fetch(`/api/user/create-account`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(params),
  });
  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.error);
  }
}