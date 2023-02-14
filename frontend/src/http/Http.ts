import { webAPIUrl } from '../AppSettings';

export interface HttpApiRequest<RequestBodyType> {
  path: string;
  payload?: RequestBodyType;
}
export interface HttpApiResponse<ResponseBodyType> {
  ok: boolean;
  body?: ResponseBodyType;
}

export const httpCall = async <ResponseBodyType, RequestBodyType = undefined>(
  config: HttpApiRequest<RequestBodyType>
): Promise<HttpApiResponse<ResponseBodyType>> => {
  const request = new Request(`${webAPIUrl}${config.path}`);
  const response = await fetch(request);
  const httpResponse: HttpApiResponse<ResponseBodyType> = { ok: false };
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
