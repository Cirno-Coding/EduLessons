from requests import get, post


print(post('http://localhost:5000/api/jobs',
           json={
               'job': 'installation of radiation protection'
           }).json())  # not full list of characters
print(post('http://localhost:5000/api/jobs', json={}).json())  # empty request
print(post('http://localhost:5000/api/jobs',
           json={
               'job': 'installing a long-distance communication antenna',
               'team_leader': 4,
               'work_size': 5,
               'collaborators': '6, 3',
               'is_finished': True
           }).json())  # cool request
print(get('http://localhost:5000/api/jobs').json())
