(function () {
  const LOGIN_PATH_HINTS = ["/login", "/auth/login", "/api/auth/login"];
  const TOKEN_FIELDS = ["token", "accessToken", "access_token", "jwt"];

  function isLoginRequest(url) {
    if (!url || typeof url !== "string") {
      return false;
    }

    const normalized = url.toLowerCase();
    return LOGIN_PATH_HINTS.some(hint => normalized.includes(hint));
  }

  function extractToken(payload) {
    if (!payload || typeof payload !== "object") {
      return null;
    }

    for (const field of TOKEN_FIELDS) {
      const value = payload[field];
      if (typeof value === "string" && value.trim()) {
        return value.trim();
      }
    }

    return null;
  }

  function applySwaggerAuth(token) {
    if (!token || !window.ui || typeof window.ui.preauthorizeApiKey !== "function") {
      return;
    }

    const bearerValue = token.toLowerCase().startsWith("bearer ") ? token : `Bearer ${token}`;
    window.ui.preauthorizeApiKey("Bearer", bearerValue);
    localStorage.setItem("swagger.jwt", bearerValue);
  }

  function waitAndApply(token) {
    let retries = 0;
    const maxRetries = 40;

    const timer = setInterval(() => {
      retries++;
      if (window.ui && typeof window.ui.preauthorizeApiKey === "function") {
        applySwaggerAuth(token);
        clearInterval(timer);
      }

      if (retries >= maxRetries) {
        clearInterval(timer);
      }
    }, 200);
  }

  const savedToken = localStorage.getItem("swagger.jwt");
  if (savedToken) {
    waitAndApply(savedToken);
  }

  const originalFetch = window.fetch;
  if (typeof originalFetch === "function") {
    window.fetch = async function (...args) {
      const response = await originalFetch.apply(this, args);

      try {
        const request = args[0];
        const requestUrl = typeof request === "string" ? request : request?.url;

        if (isLoginRequest(requestUrl)) {
          const cloned = response.clone();
          const contentType = cloned.headers.get("content-type") || "";

          if (contentType.includes("application/json")) {
            const body = await cloned.json();
            const token = extractToken(body);
            if (token) {
              waitAndApply(token);
            }
          }
        }
      } catch {
        // Intencionalmente vacío: no bloquear llamadas de Swagger UI.
      }

      return response;
    };
  }
})();
