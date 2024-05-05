import Image from "next/image";

interface Weather {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export default async function Home() {
  const weatherList: Weather[] = await fetch("http://server/weatherforecast", {
    cache: "no-cache",
  }).then((res) => res.json());

  return (
    <>
      <h1>Richiesta backend</h1>
      <ul>
        {weatherList.map((weather) => (
          <li>
            Date: {weather.date} | TemperatureC: {weather.temperatureC} |
            Summary: {weather.summary}
          </li>
        ))}
      </ul>
    </>
  );
}
