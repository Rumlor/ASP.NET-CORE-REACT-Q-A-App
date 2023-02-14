import { webAPIUrl } from '../AppSettings';

export interface HttpApiRequest<REQ> {
  path: string;
  payload?: REQ;
}
export interface HttpApiResponse<RES> {
  ok: boolean;
  body?: RES;
}

export const httpCall = async <RES, REQ = undefined>(
  config: HttpApiRequest<REQ>
): Promise<HttpApiResponse<RES>> => {
  const request = new Request(`${webAPIUrl}${config.path}`);
  const response = await fetch(request);
  const httpResponse: HttpApiResponse<RES> = { ok: false };
  if (response.ok) {
    const body = await response.json();
    httpResponse.ok = true;
    httpResponse.body = body;
    return httpResponse;
  } else {
    httpResponse.ok = response.ok;
    logHttp(request, response);
    return httpResponse;
  }
};
const logHttp = async (request: Request, response: Response): Promise<void> => {
  const contentType = response.headers.get('content-type');
  let body;
  if (contentType && contentType.indexOf('application/json') !== -1) {
    body = await response.json();
  } else {
    body = await response.text();
  }
  console.error(
    `Error while requesting ${request.method} ${request.url}`,
    body
  );
};
