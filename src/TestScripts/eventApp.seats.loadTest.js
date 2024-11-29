import http from 'k6/http';
import { sleep } from 'k6';

const scenarios = {
	one: {
		executor: 'constant-vus',
		vus: 1,
		duration: '2m'
	},
	ten: {
		executor: 'constant-vus',
		vus: 10,
		duration: '2m',
		startTime: '2m'
	},
	one_hundred: {
		executor: 'constant-vus',
		vus: 100,
		duration: '2m',
		startTime: '4m'
	},
	one_thousand: {
		executor: 'constant-vus',
		vus: 1000,
		duration: '2m',
		startTime: '6m'
	},
	ten_thousand: {
		executor: 'constant-vus',
		vus: 10000,
		duration: '2m',
		startTime: '8m'
	}
};

const { SCENARIO } = __ENV;
let scenariosOpts;
if (SCENARIO) {
	const selectedScenario = scenarios[SCENARIO];
	selectedScenario.startTime = '1m11s';
	scenariosOpts = {
		warm_up: {
			executor: 'ramping-vus',
			startVUs: 0,
			stages: [
				{ duration: '10s', target: selectedScenario.vus >= 10 ? selectedScenario.vus / 10 : selectedScenario.vus },
				{ duration: '50s', target: selectedScenario.vus }
			],
			startTime: '10s'
		},
		[SCENARIO] : selectedScenario
	};
} else {
	scenariosOpts = scenarios;
}

export let options = {
	insecureSkipTLSVerify: true,
	noConnectionReuse: false,
	scenarios: scenariosOpts
};

const eventsAppUrl = 'http://events:8080/events';

export default function() {
	const eventId = Math.floor(Math.random() * 3) + 1;
	const sectionId = (eventId-1) * 5 + Math.floor(Math.random() * 5) + 1;
	http.get(eventsAppUrl + `/${eventId}/sections/${sectionId}/seats`, { responseCallback: http.expectedStatuses(200) });
	sleep(1);
}